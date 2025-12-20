import { createFileRoute, useNavigate } from '@tanstack/react-router'
import {useEffect, useState} from "react";
import {Button, Separator, Skeleton} from "@radix-ui/themes";
import apiClient from "../apiClient.ts";
import axios from "axios";
import ReservationsTable, {type UserProfileData} from "../components/profile/ReservationsTable.tsx";

export const Route = createFileRoute('/profile/$userId')({
  component: RouteComponent,
})

function RouteComponent() {
  const navigate = useNavigate();

  const { userId } = Route.useParams();

  const [userProfileData, setUserProfileData] = useState<UserProfileData | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchUserInfo = async() => {
      try {
        const response = await apiClient.get<UserProfileData>(`/users/profile/${userId}`);
        setUserProfileData(response.data)
      } catch (error) {
        if (!axios.isAxiosError(error)) {
          setErrorMessage("Unexpected Error");
        } else {
          setErrorMessage(error.response?.data?.title);
        }
      } finally {
        setIsLoading(false);
      }
    };

    fetchUserInfo();
  },[userId]);

  if (!userProfileData && !isLoading) {
    return (
      <div className="pt-10 grow w-full flex flex-col items-center justify-center gap-y-5">
        <h2 className="text-5xl text-zinc-400">{errorMessage ?? "Internal Server Error"}</h2>
        <Button
          size="3"
          variant="surface"
          onClick={() => navigate({
            to: "/profiles"
          })}
        >
          Go back
        </Button>
      </div>
    );
  }

  return (
    <div className="w-full flex flex-col items-center justify-center gap-y-7">
      <div className="w-fit flex flex-col items-center justify-center gap-y-2">
        <Skeleton loading={isLoading}>
          <h2 className="text-5xl font-semibold">
            {!userProfileData ?
              "PlaceholderName PlaceholderSurname"
              :
              `${userProfileData.name ?? "-"} ${userProfileData.surname ?? "-"}`
            }
          </h2>
        </Skeleton>
        <Skeleton loading={isLoading}>
          <h3 className="text-xl font-light text-zinc-400">
            {!userProfileData ?
              "PlaceholderEmail"
              :
              userProfileData.email ?? "-"
            }
          </h3>
        </Skeleton>
      </div>
      <Separator size="4"/>
      <div className="w-full flex flex-col items-center justify-center gap-y-5">
        { isLoading || !userProfileData ?
          <>
            <Skeleton loading={true} width="50%" height="100px"/>
            <Skeleton loading={true} width="50%" height="100px"/>
          </>
          :
          <>
            <ReservationsTable
              title="Ongoing Reservations"
              reservations={userProfileData.ongoingReservations}
            />
            <ReservationsTable
              title="Past Reservations"
              reservations={userProfileData.pastReservations}
            />
          </>
        }
      </div>
    </div>
  );
}
