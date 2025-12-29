interface CollectionRowData {
  children: CollectionCellData[];
  props: any;
}

interface CollectionCellData {
  children: React.ReactNode;
}

interface CollectionData {
  header: CollectionCellData[][];
  body: CollectionRowData[];
  footer: CollectionCellData[][];
  pagination: {
    current: number;
    last: number;
  };
}

type CollectionProps = {
  data: CollectionData,
  onPageChange: (page: number) => void;
}

//

interface CollectionRequestProps {
  q: string;
  page: number;
  size: number;
}

interface CollectionResponseMeta {
  current_page: number;
  last_page: number;
}
