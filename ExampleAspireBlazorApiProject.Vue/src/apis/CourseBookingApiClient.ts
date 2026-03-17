import type {ICourseBooking} from "@/interfaces/ICourseBooking.ts";
import {ApiEndpoint} from "@/enums/ApiEndpoint.ts";
import {BaseApiClient} from "@/apis/base/BaseApiClient.ts";
import type {ICustomApi} from "@/interfaces/ICustomApi.ts";

class CourseBookingApiClient extends BaseApiClient implements ICustomApi {
  private _endpoint: string = `${this.baseURL}${ApiEndpoint.CourseBookings}`;

  async getAll() {
    try{
      const resp = await this.client.get<ICourseBooking[]>(this._endpoint);
      console.info("course bookings registered!", resp.data.length);
      return resp.data;
    }
    catch(err){
      console.error("no course bookings registered!", err);
      return null;
    }
  }

  async delete(id: number){
    try{
      const resp = await this.client.delete<boolean>(this._endpoint + `/${id}`);
      console.info("course booking deleted!", resp.status);
      return resp.status == 204;
    }
    catch(err){
      console.error("course booking cant be deleted", err);
      return false;
    }
  }
}

export const courseBookingApiClient = new CourseBookingApiClient();
