import { Action } from "./action";

export interface Page {
  id: string;
  name: string;
  orderNo: number;
  pageActions: Action[];
}
