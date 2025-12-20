import {createContext, useEffect, useState} from "react";
import apiClient from "../apiClient.ts";

type User = {
  id: string;
  name: string;
  surname: string;
  email: string;
}

type UserContextProps = {
  users: User[];
  user?: User;
  setUser: (user?: User) => void;
  isLoading: boolean;
}

const UserContext = createContext<UserContextProps>({
  users: [],
  user: undefined,
  setUser: () => {},
  isLoading: true
});

type UserProviderProps = {
  children: React.ReactNode[] | React.ReactNode;
}

const UserProvider = ({children}: UserProviderProps) => {
  const [user, setUser] = useState<User | undefined>(undefined);
  const [users, setUsers] = useState<User[]>([]);
  const [isLoading, setLoading] = useState(false);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        setLoading(true);
        const response = await apiClient.get<User[]>("/users/all");
        setUsers(response.data);
      } catch (error) {
        console.error(error);
      } finally {
        setLoading(false);
      }
    }

    fetchUsers();
  }, [])

  return (
    <UserContext.Provider value={{users, user, setUser, isLoading}}>
      {children}
    </UserContext.Provider>
  );
}

export {UserProvider, UserContext};
export type { User };
