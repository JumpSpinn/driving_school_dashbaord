import axios, {type AxiosInstance} from "axios";

export class BaseApiClient {
  public client: AxiosInstance;
  public baseURL: string;

  constructor() {
    this.baseURL = "http://localhost:5514/api/";
    this.client = axios.create({
      baseURL: this.baseURL,
      headers: {
        "Content-Type": "application/json"
      }
    });
  }
}
