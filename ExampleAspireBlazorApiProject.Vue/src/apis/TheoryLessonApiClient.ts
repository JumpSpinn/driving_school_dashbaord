import {BaseApiClient} from "@/apis/base/BaseApiClient.ts";
import {ApiEndpoint} from "@/enums/ApiEndpoint.ts";
import type {ITheoryLesson} from "@/interfaces/ITheoryLesson.ts";

class TheoryLessonApiClient extends BaseApiClient {
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
}

export const theoryLessonApiClient = new TheoryLessonApiClient();
