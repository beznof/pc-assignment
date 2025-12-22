import {createContext, useState} from "react";
import {Toast} from "radix-ui";
import clsx from "clsx";

type ResponseType = "error" | "success";

type ResponseToastContextProps = {
  setMessage: (message?: string, responseType?: ResponseType) => void;
};

const ResponseToastContext = createContext<ResponseToastContextProps>({
  setMessage: () => {},
});

type ResponseToastProps = {
  children: React.ReactNode[] | React.ReactNode;
};

const ResponseToastProvider = ({children}: ResponseToastProps) => {
  const [toastKey, setToastKey] = useState<number>(0);

  const [errorMessage, setErrorMessage] = useState<string|undefined|null>(null);
  const [successMessage, setSuccessMessage] = useState<string|undefined|null>(null);

  const setMessage = (message?: string, responseType?: ResponseType) => {
    if (!message || responseType === undefined) return;

    setErrorMessage(null);
    setSuccessMessage(null);

    if (responseType == "error") {
      setErrorMessage(message);
    } else {
      setSuccessMessage(message);
    }
    setToastKey(k => k + 1);
  }

  return (
    <ResponseToastContext.Provider value={{setMessage}}>
      <Toast.Provider>
        {children}
        { (errorMessage || successMessage) &&
            <>
              <Toast.Root duration={3500} key={toastKey}
                onOpenChange={(open) => {
                  if (!open) {
                   setErrorMessage(null);
                   setSuccessMessage(null);
                }}}
              >
                  <Toast.Title className={clsx("font-medium text-xl", {
                    "text-red-400": errorMessage,
                    "text-green-600": successMessage,
                  })}>
                    { errorMessage ? "Error" : "Success"}
                  </Toast.Title>
                  <Toast.Description className="font-light text-lg text-zinc-500">
                    { errorMessage ? errorMessage : successMessage }
                  </Toast.Description>
              </Toast.Root>
              <Toast.Viewport className="fixed bottom-8 right-8 px-4 py-2 rounded-lg drop-shadow-2xl min-w-[300px] ring-1 ring-zinc-200"/>
            </>
        }
      </Toast.Provider>
    </ResponseToastContext.Provider>
  );
}

export {ResponseToastProvider, ResponseToastContext};
export type {ResponseType};