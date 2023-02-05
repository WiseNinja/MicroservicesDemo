import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { MapEntitiesService } from 'src/app/services/map-entities.service';
import { MapEntityDto } from './dtos/map-entity-dto';
@Component({
  selector: 'app-map-entities',
  templateUrl: './map-entities.component.html',
  styleUrls: ['./map-entities.component.scss']
})
export class MapEntitiesComponent {
  name = new FormControl('', [Validators.required]);
  xCoordinate = new FormControl('', [Validators.required, Validators.pattern("[+-]?([0-9]*[.])?[0-9]+")]);
  yCoordinate = new FormControl('', [Validators.required, Validators.pattern("[+-]?([0-9]*[.])?[0-9]+")]);
  sendEntitySubscription!: Subscription;

  constructor(private mapEntitiesService: MapEntitiesService){

  }
  
  getErrorMessage() {
    if (this.name.hasError('required') ||
      this.xCoordinate.hasError('required') ||
      this.yCoordinate.hasError('required')) {
      return 'You must enter a value';
    }
    if (this.xCoordinate.hasError('pattern') ||
      this.yCoordinate.hasError('pattern')) {
      return 'Only numbers are a valid coordinate input'
    }
    return '';
  }

  sendEntity() {
    let mapEntityDto = new MapEntityDto();
    mapEntityDto.name = this.name.value as string;
    mapEntityDto.x = Number(this.xCoordinate.value);
    mapEntityDto.y = Number(this.yCoordinate.value);
    this.sendEntitySubscription = this.mapEntitiesService.addMapEntity(mapEntityDto).subscribe();
  }
}
