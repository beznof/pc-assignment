import {createFileRoute} from '@tanstack/react-router';
import {useContext, useEffect, useState} from "react";
import apiClient from "../apiClient.ts";
import axios, {isAxiosError} from "axios";
import {UserContext} from "../contexts/UserContext.tsx";
import {Separator} from "@radix-ui/themes";
import DeskCard, {type DeskData} from "../components/desks/DeskCard.tsx";
import ReservationCreationModal from "../components/desks/ReservationCreationModal.tsx";

export const Route = createFileRoute("/")({
  component: RouteComponent,
})

function RouteComponent() {
  const [desksData, setDesksData] = useState<DeskData[]>([]);

  const [errorMessage, setErrorMessage] = useState<string>("");
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [isReservationModalOpen, setIsReservationModalOpen] = useState(false);

  const [rangeFrom, setRangeFrom] = useState<string>(new Date().toISOString().split("T")[0]);
  const [rangeTo, setRangeTo] = useState<string>(new Date().toISOString().split("T")[0]);

  const [submitRangeFrom, setSubmitRangeFrom] = useState<string>(new Date().toISOString().split("T")[0]);
  const [submitRangeTo, setSubmitRangeTo] = useState<string>(new Date().toISOString().split("T")[0]);
  const [submitDeskId, setSubmitDeskId] = useState<number | null>(null);

  const {user} = useContext(UserContext);

  const fetchDeskDate = async () => {
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
        setErrorMessage("Unexpected Error");
      } else {
        setErrorMessage(error.response?.data?.title);
      }
    } finally {
      setIsLoading(false);
    }
  };

  const handleCancel = async (reservationId: number, todayOnly?: boolean) => {
    if (!user?.id) return;

    try {
      setIsLoading(true);
      setErrorMessage("");

      await apiClient.delete(`/reservations/${reservationId}`, {
        params: {
          "todayOnly": todayOnly ?? false,
          "userId": user.id,
        }
      })
    } catch (error) {
      if (!axios.isAxiosError(error)) {
        setErrorMessage("Unexpected Error");
      } else {
        setErrorMessage(error.response?.data?.title);
      }
    } finally {
      setIsLoading(false);
      await fetchDeskDate();
    }
  };

  const handleReservation = async () => {
    if (!user?.id || !submitDeskId) return;

    try {
      setIsLoading(true);
      setErrorMessage("");

      await apiClient.post("/reservations", {
          UserId: user.id,
          DeskId: submitDeskId,
          DateFrom: submitRangeFrom,
          DateTo: submitRangeTo,
      });
    } catch (error) {
      if (!axios.isAxiosError(error)) {
        setErrorMessage("Unexpected Error");
      } else {
        setErrorMessage(error.response?.data?.title);
      }
    } finally {
      setIsReservationModalOpen(false);
      setIsLoading(false);
      await fetchDeskDate();
    }
  }

  const openModal = (deskId: number | null) => {
    setIsReservationModalOpen(true);
    setSubmitDeskId(deskId);
  }

  useEffect(() => {
    fetchDeskDate();
  }, [user, rangeFrom, rangeTo]);

  return (
    <>
      <div className="w-full flex-col items-center justify-center gap-y-10 relative">
        <Separator/>
        <div className="grid grid-cols-2 md:grid-cols-4 gap-x-4 gap-y-4">
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
          rangeFrom={submitRangeFrom}
          rangeTo={submitRangeTo}
          setRangeFrom={setSubmitRangeFrom}
          setRangeTo={setSubmitRangeTo}
          submitReservation={handleReservation}
          isLoading={isLoading}
        />
      )}
    </>
  );
}
