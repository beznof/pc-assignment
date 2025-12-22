
type DatePickerProps = {
  label: string;
  date: string;
  setDate: (date: string) => void;
}

const DatePicker = ({label, date, setDate}: DatePickerProps) => {

  return (
    <div className="flex flex-row items-center justify-center gap-x-4">
      <label
        htmlFor="rangeFrom"
        className="text-3xl font-light"
      >
        {label}
      </label>
      <input
        id="rangeTo"
        type="date"
        value={date}
        onChange={(e) => setDate(e.target.value)}
        className="px-2 text-2xl bg-zinc-100 hover:bg-zinc-100/70 rounded-lg"
      />
    </div>
  );
}

export default DatePicker;