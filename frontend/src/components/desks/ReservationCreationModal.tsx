import CloseModalIcon from "../../assets/icons/CloseModalIcon.tsx";
import {Separator} from "@radix-ui/themes";
import DatePicker from "./DatePicker.tsx";

type ReservationCreationModalProps = {
  closeModal: () => void;
  rangeFrom: string;
  rangeTo: string;
  setRangeFrom: (newRangeFrom: string) => void;
  setRangeTo: (newRangeTo: string) => void;
  isLoading: boolean;
  submitReservation: () => void;
}

const ReservationCreationModal = ({closeModal, rangeFrom, rangeTo, setRangeFrom, setRangeTo, isLoading, submitReservation}: ReservationCreationModalProps) => {

  return (
    <div className="bg-zinc-200/70 absolute w-auto h-auto inset-0 ">
      <div className="w-full h-full relative flex justify-center items-center">
        <div className="w-full h-full absolute inset-0 z-0 cursor-pointer" onClick={closeModal}></div>
        <div className="bg-white drop-shadow-2xl px-8 py-12 rounded-lg w-[600px] flex flex-col items-center justify-center gap-y-5">
          <CloseModalIcon className="size-6 absolute left-4 top-4 cursor-pointer fill-zinc-400" onClick={closeModal}/>
          <h2 className="text-6xl text-center font-medium">Create a reservation</h2>
          <Separator size="4"/>
          <div className="flex flex-col justify-center items-end gap-y-12 py-8">
            <DatePicker
              label="From"
              date={rangeFrom}
              setDate={setRangeFrom}
            />
            <DatePicker
              label="To"
              date={rangeTo}
              setDate={setRangeTo}
            />
            <div className="w-full flex justify-center">
              <button
                className="bg-emerald-500 py-2 px-4 rounded-lg text-2xl font-medium text-white min-w-[50%] hover:bg-emerald-500/80 cursor-pointer"
                disabled={isLoading}
                onClick={submitReservation}
              >
                Submit
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ReservationCreationModal;