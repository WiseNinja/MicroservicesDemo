import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { MapsService } from 'src/app/services/maps.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { MatDialog } from '@angular/material/dialog';
import { MapUploadDialog } from './map-upload-dialog.component';
import { Subscription } from 'rxjs';
import { MissionMapDto } from './dtos/mission-map-dto';

@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
  styleUrls: ['./maps.component.scss']
})

export class MapsComponent implements OnInit {

  //maps related variables
  allMaps!: string[];
  missionMap!: string;
  selectedMap!: string;
  mapImageSource!: SafeResourceUrl;

  //subscriptions + subscribers
  mapsSubscriber!: Observable<string[]>;
  mapDataSubscriber!: Observable<string>
  missionMapSubcriber!: Observable<string>
  deleteMapSubscription!: Subscription;
  setMissionMapSubscription!: Subscription;

  //general variables
  errorMessage!: string;
  lastSelectedTabIndex!: number;

  constructor(private mapsService: MapsService, private sanitizer: DomSanitizer, public dialog: MatDialog) {
  }

  isMissionMapIconVisible(selectedMap: string): boolean {
    return selectedMap === this.missionMap;
  }

  uploadMap() {
    this.dialog.open(MapUploadDialog).afterClosed().subscribe(mapUploadDialogResult => {
      this.mapImageSource = this.sanitizer.bypassSecurityTrustResourceUrl(mapUploadDialogResult.data);
      this.allMaps.push(mapUploadDialogResult.name);
    });
  }

  selectMap(selectedMap: string) {
    this.selectedMap = selectedMap;
    this.mapDataSubscriber = this.mapsService.getMapData(selectedMap);
    this.mapDataSubscriber.subscribe({
      next: mapData => {
        let imageFileExtension = this.getImageTypeFromImageAsBase64(mapData);
        let mapImageData = "data:image/" + imageFileExtension + ";base64," + mapData;
        this.mapImageSource = this.sanitizer.bypassSecurityTrustResourceUrl(mapImageData);
      },
      error: err => {
        console.log('error during retrieving data for map selection', err);
      }
    });
  }

  deleteMap() {
    this.deleteMapSubscription = this.mapsService.deleteMap(this.selectedMap).subscribe({
      next: x => {
        if (this.missionMap == this.selectedMap) {
          this.missionMap = '';
        }
        this.removeMap(this.selectedMap);
      },
      error: err => {
        console.log('error during map deletion', err);
      }
    });
  }

  setMissionMap() {
    let missionMapDto = new MissionMapDto();
    missionMapDto.missionMapName = this.selectedMap;

    this.setMissionMapSubscription = this.mapsService.setMissionMap(missionMapDto).subscribe({
      next: x => {
        this.missionMap = this.selectedMap;
      },
      error: err => {
        console.log('error during setting mission map', err);
      }
    });
  }

  getAllMaps(){
    this.mapsSubscriber = this.mapsService.getAllMaps();
    this.mapsSubscriber.subscribe({
      next: maps => {
        this.allMaps = maps;
      },
      error: err => {
        console.log('error during retrieving all maps to display', err);
      }
    });
  }

  getMissionMap(){
    this.missionMapSubcriber = this.mapsService.getMissionMap();
    this.missionMapSubcriber.subscribe({
      next: missionMap => {
        this.missionMap = missionMap;
      },
      error: err => {
        console.log('error during retrieving the mission map', err);
      }
    });
  }

  ngOnInit(): void {
    this.getAllMaps();
    this.getMissionMap();
  }

  private removeMap(mapToDelete: string) {
    this.allMaps.forEach((value, index) => {
      if (value == mapToDelete) {
        this.allMaps.splice(index, 1);
        this.lastSelectedTabIndex = index;
      }
    });

    if (this.lastSelectedTabIndex === 0) {
      this.selectedMap = this.allMaps[0];
    }
    else {
      this.selectedMap = this.allMaps[this.lastSelectedTabIndex - 1]
    }
    this.mapImageSource = this.sanitizer.bypassSecurityTrustResourceUrl("");
  }

  private getImageTypeFromImageAsBase64(base64String: string): string {

    let fileHeader = new Map();

    //get the first 3 char of base64
    fileHeader.set("/9j", "jpg")
    fileHeader.set("iVB", "png")
    fileHeader.set("Qk0", "bmp")
    fileHeader.set("SUk", "tiff")

    let imageType = ""

    fileHeader.forEach((v, k) => {
      if (k == base64String.slice(0, 3)) {
        imageType = v
      }
    })

    //if file is not supported
    if (imageType == "") {
      imageType = "unknown file"
    }

    //return map value
    return imageType;
  }
}


