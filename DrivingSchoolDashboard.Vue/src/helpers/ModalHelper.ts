import type {IModalOptions} from "@/interfaces/IModalOptions.ts";

export class ModalHelper {
  public static readonly DefaultOptions: IModalOptions = {
    backdropClick: false,
    closeWithEsc: true,
  };

  public static readonly InfoOptions: IModalOptions = {
    backdropClick: true,
    closeWithEsc: true,
  };
}
