import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import './index.css'
import "@radix-ui/themes/styles.css";
import {router} from "./router.tsx";
import {RouterProvider} from "@tanstack/react-router";
import {Theme} from "@radix-ui/themes";
import {UserProvider} from "./contexts/UserContext.tsx";

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <UserProvider>
      <Theme accentColor="tomato" grayColor="gray">
        <RouterProvider router={router}/>
      </Theme>
    </UserProvider>
  </StrictMode>
)
