import DeskIcon from "../../assets/icons/DeskIcon.tsx";
import ProfileIcon from "../../assets/icons/ProfileIcon.tsx";
import NavItem from "./NavItem.tsx";
import apiClient from "../../apiClient.ts";
import UserSelect from "./UserSelect.tsx";

const Navbar = () => {

  apiClient.get("/users/all")
  .then((response) => {console.log(response)});

  return (
    <div className="w-full h-full py-2 px-5 flex items-center justify-center border-b border-stone-200">
      <div className="h-full flex flex-col md:flex-row items-center justify-between w-full">
        <div className="flex flexrow gap-x-4 w-fit">
          <NavItem
            title="Desks"
            icon={<DeskIcon className="size-6 fill-stone-800"/>}
            to="/"
          />
          <NavItem
            title="Profiles"
            icon={<ProfileIcon className="size-6 fill-stone-800"/>}
            to="/profiles"
          />
        </div>
        <UserSelect/>
      </div>
    </div>
  );
}

export default Navbar;