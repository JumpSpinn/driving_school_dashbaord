import type {ICourseBooking} from "@/interfaces/ICourseBooking.ts";
import {ApiEndpoint} from "@/enums/ApiEndpoint.ts";
import {BaseApiClient} from "@/apis/base/BaseApiClient.ts";

class CourseBookingApiClient extends BaseApiClient {
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
}

export const courseBookingApiClient = new CourseBookingApiClient();
