import {BaseApiClient} from "@/apis/base/BaseApiClient.ts";
import {ApiEndpoint} from "@/enums/ApiEndpoint.ts";
import type {IDrivingSchool} from "@/interfaces/IDrivingSchool.ts";

class DrivingSchoolApiClient extends BaseApiClient {
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
}

export const drivingSchoolApiClient = new DrivingSchoolApiClient();
