import { createFileRoute } from '@tanstack/react-router'
import {useContext} from "react";
import {UserContext} from "../contexts/UserContext.tsx";
import {Card, Skeleton} from "@radix-ui/themes";
import {Link} from "@tanstack/react-router";

export const Route = createFileRoute('/profiles')({
  component: ProfilesPage,
})

function ProfilesPage() {
  const {users, user} = useContext(UserContext);

  const isLoading = false;

  const filteredUsers = users.filter(u =>
    u.id !== user?.id
  );

  return (
    <div className="w-full flex flex-col gap-y-12">
      <div className="min-w-[300px] w-fit mx-auto">
        <Skeleton loading={isLoading}>
          {user ?
            <Link
              to="/profile/$userId"
              params={{
                userId: user.id
              }}
              className="group"
            >
              <Card
                variant="surface"
                className="group-hover:ring-1 group-hover:ring-zinc-300"
              >
                <h2 className="text-2xl font-bold text-black">
                  My Profile
                </h2>
                <h3 className="text-lg">
                  {user.name } {user.surname} <span className="font-extralight text-zinc-600">({user.email})</span>
                </h3>
              </Card>
            </Link>
          :
            <Card>
              <h2 className="text-2xl font-bold text-black">
                My Profile
              </h2>
              <h3 className="font-extralight text-lg text-zinc-400 ">
                Please select a user
              </h3>
            </Card>
          }
        </Skeleton>
      </div>
      <div className="w-fit mx-auto grid grid-cols-2 gap-y-4 gap-x-4">
        {
          filteredUsers.map(u => (
            <Link
              to="/profile/$userId"
              params={{
                userId: u.id,
              }}
              className="group"
            >
              <Card className="w-auto h-full group-hover:ring-1 group-hover:ring-zinc-300">
                <h3 className="text-lg">
                  {u?.name} {u?.surname} <span className="font-extralight text-zinc-600">({u.email})</span>
                </h3>
              </Card>
            </Link>
          ))
        }
      </div>
    </div>
  );
}
