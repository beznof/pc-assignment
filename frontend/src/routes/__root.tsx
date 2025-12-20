import {createRootRoute, Outlet} from "@tanstack/react-router";
import Navbar from "../components/layout/Navbar.tsx";

export const Route = createRootRoute({
  component: Layout,
})

function Layout() {
  return (
    <div className="w-full h-full flex flex-col bg-white">
      <Navbar/>
      <main className="flex-1 overflow-auto flex">
        <div className="w-full max-w-5xl px-4 py-6 mx-auto">
          <Outlet/>
        </div>
      </main>
    </div>
  );
}