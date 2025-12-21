import MaintenanceIcon from "../../assets/icons/MaintenanceIcon.tsx";

const UnderMaintenance = () => {

  return (
    <div className="w-full h-full flex flex-col items-center justify-center gap-y-3">
      <MaintenanceIcon className="w-[50%] fill-none stroke-white"/>
      <p className="font-light">Under Maintenance</p>
    </div>
  );
}

export default UnderMaintenance;