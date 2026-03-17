import {BaseApiClient} from "@/apis/base/BaseApiClient.ts";
import {ApiEndpoint} from "@/enums/ApiEndpoint.ts";
import type {IDrivingSchool} from "@/interfaces/IDrivingSchool.ts";
import type {ICustomApi} from "@/interfaces/ICustomApi.ts";

class DrivingSchoolApiClient extends BaseApiClient implements ICustomApi {
  private _endpoint: string = `${this.baseURL}${ApiEndpoint.DrivingSchool}`;

  async getAll() {
    try{
      const resp = await this.client.get<IDrivingSchool[]>(this._endpoint);
      console.info("driving schools registered!", resp.data.length);
      return resp.data;
    }
    catch(err){
      console.error("no driving schools registered!", err);
      return null;
    }
  }

  async delete(id: number){
    try{
      const resp = await this.client.delete<boolean>(this._endpoint + `/${id}`);
      console.info("driving school deleted!", resp.status);
      return resp.status == 204;
    }
    catch(err){
      console.error("driving school cant be deleted", err);
      return false;
    }
  }
}

export const drivingSchoolApiClient = new DrivingSchoolApiClient();
