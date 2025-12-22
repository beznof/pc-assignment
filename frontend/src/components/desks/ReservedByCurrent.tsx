import clsx from "clsx";


type ReservedByCurrentProps = {
  isCancellableToday: boolean;
  isCancellable: boolean;
  fromRange: string;
  toRange: string;
  reservationId: number;
  onCancel: (reservationId: number, todayOnly?: boolean) => void;
  isLoading: boolean;
};

const ReservedByCurrent = ({isCancellableToday, isCancellable, fromRange, toRange, reservationId, onCancel, isLoading}: ReservedByCurrentProps) => {
  return (
    <div className="w-full h-full flex flex-col items-center justify-between gap-y-3">
      <div className="flex flex-col items-center gap-y-2">
        <p className="font-light">Reserved by <span className="font-medium">You</span></p>
        <p className="font-extralight text-xs">
          {`${fromRange} - ${toRange}`}
        </p>
      </div>
      { isCancellableToday && isCancellable ?
        <div className="max-w-full flex flex-row gap-x-3 text-sm">
          <button
            className={clsx("bg-white hover:bg-white/80 text-black font-medium rounded-lg px-2 py-1 shadow-lg cursor-pointer", {
              'bg-white/60 hover:bg-white/60 cursor-none': isLoading,
            })}
            onClick={() => onCancel(reservationId)}
            disabled={isLoading}
          >
            Cancel
          </button>
          <button
            className={clsx("bg-white hover:bg-white/80 text-black font-medium rounded-lg px-2 py-1 shadow-lg cursor-pointer", {
              'bg-white/60 hover:bg-white/60 cursor-none': isLoading,
            })}
            onClick={() => onCancel(reservationId, true)}
            disabled={isLoading}
          >
            Cancel for Today
          </button>
        </div>
        : isCancellable ?
          <button
            className={clsx("bg-white hover:bg-white/80 text-black font-medium rounded-lg px-2 py-1 shadow-lg cursor-pointer", {
              'bg-white/60 hover:bg-white/60 cursor-none': isLoading,
            })}
            onClick={() => onCancel(reservationId)}
            disabled={isLoading}
          >
            Cancel
          </button>
        :
          <></>
      }
    </div>
  );
}

export default ReservedByCurrent;