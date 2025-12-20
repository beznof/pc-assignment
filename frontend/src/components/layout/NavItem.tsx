import {Link} from "@tanstack/react-router";

type NavItemProps = {
  title: string;
  to: string;
  icon: React.ReactNode;
};

const NavItem = ({ title, to, icon }: NavItemProps) => {
  return (
    <Link
      to={to}
      className="flex flex-row items-center justify-center hover:bg-stone-200 py-1 px-2 rounded-lg gap-x-2"
    >
      {icon}
      <p className="text-lg font-light text-black">
        {title}
      </p>
    </Link>
  );
};

export default NavItem;

