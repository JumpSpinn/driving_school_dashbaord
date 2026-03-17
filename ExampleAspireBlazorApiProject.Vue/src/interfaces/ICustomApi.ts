export interface ICustomApi {
  getAll() : Promise<any>;
  delete(id: number) : Promise<boolean>;
}
