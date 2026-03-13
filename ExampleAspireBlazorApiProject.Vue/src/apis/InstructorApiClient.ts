import {BaseApiClient} from "@/apis/base/BaseApiClient.ts";
import {ApiEndpoint} from "@/enums/ApiEndpoint.ts";
import type {IInstructor} from "@/interfaces/IInstructor.ts";

class InstructorApiClient extends BaseApiClient {
  private _endpoint: string = `${this.baseURL}${ApiEndpoint.Instructor}`;

  async getAll() {
    try{
      const resp = await this.client.get<IInstructor[]>(this._endpoint);
      console.info("instructors registered!", resp.data.length);
      return resp.data;
    }
    catch(err){
      console.error("no instructors registered!", err);
      return null;
    }
  }
}

export const instructorApiClient = new InstructorApiClient();
