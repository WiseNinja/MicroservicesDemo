import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MapToUploadDto } from '../components/maps/map-to-upload-dto';
import { MissionMapDto } from '../components/maps/mission-map-dto';

@Injectable({
  providedIn: 'root'
})
export class MapsService {

  mockMaps: string[] = [];

  baseUrl = 'http://localhost:5003';

  constructor(private http: HttpClient) { }

  getAllMaps(): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + '/api/MapsRepository/GetAllMapNames');
  }

  getMapData(mapName: string): Observable<string> {
    const requestOptions: Object = {
      params: new HttpParams().set("mapName", mapName),
      responseType: 'text'
    }
    let result = this.http.get<string>(this.baseUrl + '/api/MapsRepository/GetMapDataByMapName',  requestOptions);
    return result;
  }

  uploadMap(mapToUploadDto: MapToUploadDto): Observable<any> {

    this.mockMaps.push(mapToUploadDto.name);
    return this.http.post<MapToUploadDto>(this.baseUrl + '/api/MapsRepository/UploadMap', mapToUploadDto);
  }

  deleteMap(mapName: string): Observable<any> {
    return this.http.delete<any>(this.baseUrl + '/api/MapsRepository/DeleteMap/' + mapName);
  }

  setMissionMap(missionMapDto: MissionMapDto){
    return this.http.post<any>(this.baseUrl + '/api/MapsRepository/SetMissionMap', missionMapDto);
  }

  getMissionMap(): Observable<string> {
    const requestOptions: Object = {
      responseType: 'text'
    }
    return this.http.get<string>(this.baseUrl + '/api/MapsRepository/GetMissionMapName', requestOptions);
  }

}
