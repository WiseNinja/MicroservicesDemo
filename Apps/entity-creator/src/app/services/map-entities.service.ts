import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MapEntityDto } from '../components/map-entities/map-entity-dto';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class MapEntitiesService {
  
  baseUrl = 'http://localhost:5003';

  constructor(private http: HttpClient) { 
  }

  addMapEntity(mapEntityDto: MapEntityDto): Observable<any> {
    return this.http.post<any>(this.baseUrl + '/api/MapPoints/SetNewMapPoint', mapEntityDto);
  }
}
