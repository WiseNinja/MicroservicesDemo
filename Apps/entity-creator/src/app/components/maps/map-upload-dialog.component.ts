import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { MapsService } from 'src/app/services/maps.service';
import { MapToUploadDto } from './dtos/map-to-upload-dto';

@Component({
  selector: 'map-upload-dialog',
  templateUrl: 'map-upload-dialog.component.html',
  styleUrls: ['./map-upload-dialog.component.scss']
})
export class MapUploadDialog {

  mapName!: string;
  mapToUpload!: File; // hold our file
  uploadMapSubscription!: Subscription;
  uploadedMap!: MapToUploadDto;

  constructor(private mapsService: MapsService, private mapUploadDialogRef: MatDialogRef<MapUploadDialog>) {

  }

  openFileBrowser() {
    document.getElementById("fileInput")?.click();
  }


  fileChange(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.mapToUpload = file;
    }
  }

  uploadMap() {
    console.log('sending this to server', this.mapToUpload, this.mapName);
    let mapToUploadDto = new MapToUploadDto();
    mapToUploadDto.name = this.mapName;

    let reader = new FileReader();
    reader.onload = () => {
      let readerResult = reader.result;
      if (readerResult) {
        mapToUploadDto.data = readerResult.toString();
      }
      this.uploadMapSubscription = this.mapsService.uploadMap(mapToUploadDto).subscribe({
        next: map => {
          this.uploadedMap = map;
          this.mapUploadDialogRef.close(this.uploadedMap);
        }
      });
    }
    reader.readAsDataURL(this.mapToUpload);
  }
}
