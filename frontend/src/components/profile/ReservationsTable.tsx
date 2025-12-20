import {Table} from "@radix-ui/themes";

type ReservationsTableProps = {
  title: string,
  reservations: Reservation[],
}

type Reservation = {
  code: string,
  fromDate: string,
  toDate: string,
}

export type UserProfileData = {
  name: string,
  surname: string,
  email: string,
  ongoingReservations: Reservation[],
  pastReservations: Reservation[],
};

const ReservationsTable = ({title, reservations}: ReservationsTableProps) => {
  return (
    <div className="w-full flex flex-col items-start px-6 gap-y-2">
      <h4 className="text-xl font-medium">
        {title}
      </h4>
      <Table.Root className="w-full px-8">
        <Table.Header>
          <Table.Row>
            <Table.ColumnHeaderCell>Desk Code</Table.ColumnHeaderCell>
            <Table.ColumnHeaderCell>Reservation Start</Table.ColumnHeaderCell>
            <Table.ColumnHeaderCell>Reservation End</Table.ColumnHeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {
            reservations.map((r, i) => (
              <Table.Row key={`${i}`}>
                <Table.Cell>{r.code ?? "-"}</Table.Cell>
                <Table.Cell>{r.fromDate ?? "-"}</Table.Cell>
                <Table.Cell>{r.toDate ?? "-"}</Table.Cell>
              </Table.Row>
            ))
          }
        </Table.Body>
      </Table.Root>
    </div>
  );
};

export default ReservationsTable;