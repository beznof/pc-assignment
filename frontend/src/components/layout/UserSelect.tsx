import { Select } from "@radix-ui/themes";
import {useContext} from "react";
import {UserContext} from "../../contexts/UserContext.tsx";

const UserSelect = () => {
  const {users, user, setUser} = useContext(UserContext);

  return (
    <Select.Root
      value={user?.id}
      size="3"
      onValueChange={ (value: string) => {
        setUser(users.find((user) => user.id === value));
      }}
    >
      <Select.Trigger
        placeholder="Select a user"
        variant="soft"
        radius="large"
        className="bg-transparent! hover:bg-stone-200! text-black! focus:outline-none! [&_span]:text-black!"
      />
      <Select.Content position="popper">
        <Select.Group>
          {
            users.map(user =>
              (
                <Select.Item
                  value={user.id}
                >
                  {user.name} {user.surname} ({user.email})
                </Select.Item>
              )
            )
          }
        </Select.Group>
      </Select.Content>
    </Select.Root>
  );
}

export default UserSelect;