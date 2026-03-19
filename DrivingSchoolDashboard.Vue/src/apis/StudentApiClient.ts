import {BaseApiClient} from "@/apis/base/BaseApiClient.ts";
import {ApiEndpoint} from "@/enums/ApiEndpoint.ts";
import type {IStudent, IStudentBase, IStudentUpdate} from "@/interfaces/IStudent.ts";
import type {ICustomApi} from "@/interfaces/ICustomApi.ts";

class StudentApiClient extends BaseApiClient implements ICustomApi {
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

  async create(data: IStudentBase){
    try{
      const resp = await this.client.post<IStudent>(this._endpoint, data);
      console.info("student created!", resp.data);
      return resp.data;
    }
    catch(err){
      console.error("student cant be created", err);
      return null;
    }
  }

  async update(data: IStudentUpdate){
    try{
      const resp = await this.client.put<IStudent>(this._endpoint, data);
      console.info("student updated!", resp.data);
      return resp.data;
    }
    catch(err){
      console.error("student cant be updated", err);
    }
  }

  async delete(id: number){
    try{
      const resp = await this.client.delete<boolean>(this._endpoint + `/${id}`);
      console.info("student deleted!", resp.status);
      return resp.status == 204;
    }
    catch(err){
      console.error("student cant be deleted", err);
      return false;
    }
  }
}

export const studentApiClient = new StudentApiClient();
