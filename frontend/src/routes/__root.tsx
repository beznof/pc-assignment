import {createRootRoute, Outlet} from "@tanstack/react-router";
import Navbar from "../components/layout/Navbar.tsx";
import {UserProvider} from "../contexts/UserContext.tsx";
import {Theme} from "@radix-ui/themes";
import {ResponseToastProvider} from "../contexts/ResponseToastContext.tsx";

export const Route = createRootRoute({
  component: Layout,
})

function Layout() {
  return (
    <UserProvider>
      <ResponseToastProvider>
        <Theme accentColor="tomato" grayColor="gray">
          <div className="w-full h-full flex flex-col bg-white">
            <Navbar/>
            <main className="flex-1 overflow-auto flex">
              <div className="w-full max-w-5xl px-4 py-6 mx-auto">
                <Outlet/>
              </div>
            </main>
          </div>
        </Theme>
      </ResponseToastProvider>
    </UserProvider>
  );
}