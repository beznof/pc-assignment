
type UnreservedProps = {
  openModal: (deskId: number | null) => void;
  submitDeskId: number | null;
};

const Unreserved = ({openModal, submitDeskId}: UnreservedProps) => {
  return (
    <div className="w-full h-full flex flex-col items-center justify-center">
      <button
        className="bg-white hover:bg-white/80 text-black font-medium rounded-lg px-4 py-2 shadow-lg cursor-pointer text-xl"
        onClick={() => openModal(submitDeskId)}
      >
        Reserve
      </button>
    </div>
  );
}

export default Unreserved;