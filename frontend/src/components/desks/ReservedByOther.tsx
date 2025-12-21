import ReservedIcon from "../../assets/icons/ReservedIcon.tsx";

type ReservedByOtherProps = {
  reservedBy: string;
  fromRange: string;
  toRange: string;
}

const ReservedByOther = ({reservedBy, fromRange, toRange}: ReservedByOtherProps) => {
  return (
    <div className="w-full h-full flex flex-col items-center justify-center gap-y-3">
      <ReservedIcon className="w-[50%] fill-white"/>
      <p className="font-light text-center line-clamp-2 whitespace-pre-wrap">
        {"Reserved by:\n"}
        <span className="font-medium">{reservedBy ?? "-"}</span>
      </p>
      <p className="font-extralight text-xs">
        {`${fromRange} - ${toRange}`}
      </p>
    </div>
  );
}

export default ReservedByOther;