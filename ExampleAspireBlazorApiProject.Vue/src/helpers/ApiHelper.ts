import type {ICustomApi} from "@/interfaces/ICustomApi.ts";

export class ApiHelper {
  public static async getAll(api: ICustomApi) {
    const resp = await api.getAll();
    return resp ?? [];
  }

  public static async delete(id: number, api: ICustomApi) {
    return await api.delete(id);
  }
}
