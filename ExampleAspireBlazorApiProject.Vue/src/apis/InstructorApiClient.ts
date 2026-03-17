import {BaseApiClient} from "@/apis/base/BaseApiClient.ts";
import {ApiEndpoint} from "@/enums/ApiEndpoint.ts";
import type {IInstructor} from "@/interfaces/IInstructor.ts";
import type {ICustomApi} from "@/interfaces/ICustomApi.ts";

class InstructorApiClient extends BaseApiClient implements ICustomApi {
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

  async delete(id: number){
    try{
      const resp = await this.client.delete<boolean>(this._endpoint + `/${id}`);
      console.info("instructor deleted!", resp.status);
      return resp.status == 204;
    }
    catch(err){
      console.error("instructor cant be deleted", err);
      return false;
    }
  }
}

export const instructorApiClient = new InstructorApiClient();
