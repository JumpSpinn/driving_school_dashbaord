import {BaseApiClient} from "@/apis/base/BaseApiClient.ts";
import {ApiEndpoint} from "@/enums/ApiEndpoint.ts";
import type {IStudent} from "@/interfaces/IStudent.ts";

class StudentApiClient extends BaseApiClient {
  private _endpoint: string = `${this.baseURL}${ApiEndpoint.Student}`;

  async getAll() {
    try{
      const resp = await this.client.get<IStudent[]>(this._endpoint);
      console.info("students registered!", resp.data.length);
      return resp.data;
    }
    catch(err){
      console.error("no students registered!", err);
      return null;
    }
  }
}

export const studentApiClient = new StudentApiClient();
