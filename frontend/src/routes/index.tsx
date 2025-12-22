import {createFileRoute} from '@tanstack/react-router';
import {useContext, useEffect, useState} from "react";
import apiClient from "../apiClient.ts";
import axios, {isAxiosError} from "axios";
import {UserContext} from "../contexts/UserContext.tsx";
import {Separator} from "@radix-ui/themes";
import DeskCard, {type DeskData} from "../components/desks/DeskCard.tsx";
import ReservationCreationModal from "../components/desks/ReservationCreationModal.tsx";
import DatePicker from "../components/desks/DatePicker.tsx";
import {ResponseToastContext} from "../contexts/ResponseToastContext.tsx";

export const Route = createFileRoute("/")({
  component: DesksReservationPage,
})

function DesksReservationPage() {
  const [desksData, setDesksData] = useState<DeskData[]>([]);

  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [isReservationModalOpen, setIsReservationModalOpen] = useState(false);

  const [rangeFrom, setRangeFrom] = useState<string>(new Date().toISOString().split("T")[0]);
  const [rangeTo, setRangeTo] = useState<string>(new Date().toISOString().split("T")[0]);

  const [submitDeskId, setSubmitDeskId] = useState<number | null>(null);

  const {user} = useContext(UserContext);
  const {setMessage} = useContext(ResponseToastContext);

  const fetchDeskData = async () => {
    if (!user) return

    try {
      const response = await apiClient.get<DeskData[]>("/desks", {
        params: {
          "userId": user?.id,
          rangeFrom,
          rangeTo,
        },
      });
      setDesksData(response.data);
    } catch (error) {
      if (!isAxiosError(error)) {
        setMessage("Unexpected Error", "error");
      } else {
        setMessage(error.response?.data?.title, "error");
      }
      setDesksData([]);
    } finally {
      setIsLoading(false);
    }
  };

  const handleCancel = async (reservationId: number, todayOnly?: boolean) => {
    if (!user?.id) return;

    try {
      setIsLoading(true);

      await apiClient.delete(`/reservations/${reservationId}`, {
        params: {
          "todayOnly": todayOnly ?? false,
          "userId": user.id,
        }
      })

      setMessage("Successfully cancelled", "success");
    } catch (error) {
      if (!axios.isAxiosError(error)) {
        setMessage("Unexpected Error", "error");
      } else {
        setMessage(error.response?.data?.title, "error");
      }
    } finally {
      setIsLoading(false);
      await fetchDeskData();
    }
  };

  const handleReservation = async () => {
    if (!user?.id || !submitDeskId) return;

    try {
      setIsLoading(true);

      await apiClient.post("/reservations", {
          UserId: user.id,
          DeskId: submitDeskId,
          DateFrom: rangeFrom,
          DateTo: rangeTo,
      });

      setMessage("Successfully reserved", "success");
    } catch (error) {
      if (!axios.isAxiosError(error)) {
        setMessage("Unexpected Error", "error");
      } else {
        setMessage(error.response?.data?.title, "error");
      }
    } finally {
      setIsReservationModalOpen(false);
      setIsLoading(false);
      await fetchDeskData();
    }
  }

  const openModal = (deskId: number | null) => {
    setIsReservationModalOpen(true);
    setSubmitDeskId(deskId);
  }

  useEffect(() => {
    fetchDeskData();
  }, [user, rangeFrom, rangeTo]);

  if (!user) {
    return (
      <div className="w-full flex flex-col justify-center items-center gap-y-5 my-20 text-center">
        <h2 className="font-semibold text-5xl">
          User not selected
        </h2>
        <h3 className="font-light text-zinc-500 text-2xl">
          Please select a user
        </h3>
      </div>
    )
  }

  return (
    <>
      <div className="w-full flex flex-col items-center justify-center gap-y-5 relative">
        <h2 className="text-5xl font-semibold text-center">Desk Reservation</h2>
        <Separator size="4"/>
        <div className="w-full flex flex-col md:flex-row items-center justify-center gap-x-5">
          <DatePicker
            label="From"
            date={rangeFrom}
            setDate={setRangeFrom}
          />
          <DatePicker
            label="To"
            date={rangeTo}
            setDate={setRangeTo}
          />
        </div>
        <div className="grid grid-cols-2 md:grid-cols-4 gap-x-4 gap-y-4 w-full">
          {desksData.map(d => (
            <DeskCard
              desk={d}
              isLoading={isLoading}
              onCancel={handleCancel}
              openModal={openModal}
              submitDeskId={d.deskId}
            />
          ))}
        </div>
      </div>

      { isReservationModalOpen && (
        <ReservationCreationModal
          closeModal={() => {setIsReservationModalOpen(false);}}
          rangeFrom={rangeFrom}
          rangeTo={rangeTo}
          setRangeFrom={setRangeFrom}
          setRangeTo={setRangeTo}
          submitReservation={handleReservation}
          isLoading={isLoading}
        />
      )}
    </>
  );
}
