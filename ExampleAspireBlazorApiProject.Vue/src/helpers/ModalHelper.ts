import type {IModalOptions} from "@/interfaces/IModalOptions.ts";

export class ModalHelper {
  public static readonly DefaultOptions: IModalOptions = {
    backdropClick: false,
  };

  public static readonly InfoOptions: IModalOptions = {
    backdropClick: true,
  };
}
