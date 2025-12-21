import clsx from "clsx";
import {useState} from "react";
import UnderMaintenance from "./UnderMaintenance.tsx";
import ReservedByOther from "./ReservedByOther.tsx";
import ReservedByCurrent from "./ReservedByCurrent.tsx";
import Unreserved from "./Unreserved.tsx";

type DeskData = {
  deskId: number,
  deskCode: string,
  isDeskUnderMaintenance: boolean,
  isDeskReserved: boolean,
  isReservedByCurrentUser: boolean,
  isCancellableToday: true,
  reservedBy: string,
  reservationId: number,
  fromRange: string,
  toRange: string,
};

type DeskCardProps = {
  desk: DeskData;
  isLoading: boolean;
  onCancel: (reservationId: number, todayOnly?: boolean) => void;
  openModal: (deskId: number | null) => void;
  submitDeskId: number | null;
}

const DeskCard = ({desk, isLoading, onCancel, openModal, submitDeskId}: DeskCardProps) => {
  const [cardHovered, setCardHovered] = useState<boolean>(false);

  return (
      <div className={clsx("relative w-auto min-h-25 rounded-lg text-white shadow-lg aspect-square flex items-center justify-center", {
        "bg-yellow-500 ": desk.isDeskUnderMaintenance,
        "bg-emerald-500": desk.isReservedByCurrentUser,
        "bg-red-500": desk.isDeskReserved && !desk.isReservedByCurrentUser,
        "bg-transparent text-zinc-900!": !desk.isDeskReserved && !desk.isDeskUnderMaintenance,
      })}
           onMouseEnter={() => setCardHovered(true)}
           onMouseLeave={() => setCardHovered(false)}
      >
        <p className="text-9xl font-medium text-shadow-white text-shadow-sm">
          {desk.deskCode}
        </p>
        {cardHovered &&
          <div className="absolute w-full h-full gap-y-12 bg-zinc-400/60 rounded-lg backdrop-blur-md p-4 transition-opacity duration-200">
            { (() => {
              if (desk.isDeskUnderMaintenance) {
                return <UnderMaintenance/>
              } else if (desk.isDeskReserved && !desk.isReservedByCurrentUser) {
                return <ReservedByOther
                  reservedBy={desk.reservedBy ?? "-"}
                  fromRange={desk.fromRange ?? "..."}
                  toRange={desk.toRange ?? "..."}
                />
              } else if (desk.isReservedByCurrentUser) {
                return <ReservedByCurrent
                  reservationId={desk.reservationId}
                  isCancellableToday={desk.isCancellableToday}
                  fromRange={desk.fromRange ?? "..."}
                  toRange={desk.toRange ?? "..."}
                  isLoading={isLoading}
                  onCancel={onCancel}
                />
              } else if (!desk.isDeskReserved && !desk.isDeskUnderMaintenance) {
                return <Unreserved openModal={openModal} submitDeskId={submitDeskId} />
              }
            })()}
          </div>
        }
      </div>
  )
}

export type { DeskData};
export default DeskCard;