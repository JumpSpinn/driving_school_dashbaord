import {BaseApiClient} from "@/apis/base/BaseApiClient.ts";
import {ApiEndpoint} from "@/enums/ApiEndpoint.ts";
import type {
  ITheoryLesson,
  ITheoryLessonBase,
  ITheoryLessonUpdate
} from "@/interfaces/ITheoryLesson.ts";
import type {ICustomApi} from "@/interfaces/ICustomApi.ts";

class TheoryLessonApiClient extends BaseApiClient implements ICustomApi {
  private _endpoint: string = `${this.baseURL}${ApiEndpoint.TheoryLesson}`;

  async getAll() {
    try{
      const resp = await this.client.get<ITheoryLesson[]>(this._endpoint);
      console.info("theory lessons registered!", resp.data.length);
      return resp.data;
    }
    catch(err){
      console.error("no theory lessons registered!", err);
      return null;
    }
  }

  async create(data: ITheoryLessonBase){
    try{
      const resp = await this.client.post<ITheoryLesson>(this._endpoint, data);
      console.info("theory lesson created!", resp.data);
      return resp.data;
    }
    catch(err){
      console.error("theory lesson cant be created", err);
      return null;
    }
  }

  async update(data: ITheoryLessonUpdate){
    try{
      const resp = await this.client.put<ITheoryLesson>(this._endpoint, data);
      console.info("theory lesson updated!", resp.data);
      return resp.data;
    }
    catch(err){
      console.error("theory lesson cant be updated", err);
    }
  }

  async delete(id: number){
    try{
      const resp = await this.client.delete<boolean>(this._endpoint + `/${id}`);
      console.info("theory lesson deleted!", resp.status);
      return resp.status == 204;
    }
    catch(err){
      console.error("theory lesson cant be deleted", err);
      return false;
    }
  }
}

export const theoryLessonApiClient = new TheoryLessonApiClient();
