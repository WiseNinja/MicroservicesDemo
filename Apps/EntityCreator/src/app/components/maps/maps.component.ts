import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { MapsService } from 'src/app/services/maps.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { MatDialog } from '@angular/material/dialog';
import { MapUploadDialog } from './map-upload-dialog.component';
import { Subscription } from 'rxjs';
import { MissionMapDto } from './mission-map-dto';

@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
  styleUrls: ['./maps.component.scss']
})
export class MapsComponent implements OnInit {

  missionMap!: string;
  selectedMap!: string;
  imageSource!: SafeResourceUrl;
  maps!: Observable<string[]>;
  mapData!: Observable<string>
  missionMapSubcriber!: Observable<string>
  actualMaps!: string[];
  deleteMapSub!: Subscription;
  setMissionMapSub!: Subscription;
  errorMessage: any;
  lastSelectedIndex!: number;

  constructor(private mapsService: MapsService, private sanitizer: DomSanitizer, public dialog: MatDialog) {
  }
  getWidth(): any {
    if (document.body.offsetWidth < 850) {
      return '90%';
    }

    return 850;
  }
  isMissionMapIconVisible(selectedMap: string): boolean {
    return selectedMap === this.missionMap;
  }

  uploadMap() {
    this.dialog.open(MapUploadDialog).afterClosed().subscribe(result => {
      this.imageSource = this.sanitizer.bypassSecurityTrustResourceUrl(result.data);
      this.actualMaps.push(result.name);
    });
  }

  selectMap(selectedMap: string) {
    this.selectedMap = selectedMap;
    this.mapData = this.mapsService.getMapData(selectedMap);
    this.mapData.subscribe({
      next: mapData => {
        let extension = this.getImageTypeFromBase64String(mapData);
        let mapImageData = "data:image/" + extension + ";base64," + mapData;
        this.imageSource = this.sanitizer.bypassSecurityTrustResourceUrl(mapImageData);
      },
      error: err => this.errorMessage = err
    });
  }

  deleteMap() {
    this.deleteMapSub = this.mapsService.deleteMap(this.selectedMap).subscribe({
      next: x => {
        if (this.missionMap == this.selectedMap) {
          this.missionMap = '';
        }
        this.removeMap(this.selectedMap);
      }
    });
  }

  setMissionMap() {
    let missionMapDto = new MissionMapDto();
    missionMapDto.name = this.selectedMap;

    this.setMissionMapSub = this.mapsService.setMissionMap(missionMapDto).subscribe({
      next: x => {
        this.missionMap = this.selectedMap;
      }
    });
  }

  getAllMaps(){
    this.maps = this.mapsService.getAllMaps();
    this.maps.subscribe({
      next: maps => {
        this.actualMaps = maps;
      },
      error: err => this.errorMessage = err
    });
  }

  getMissionMap(){
    this.missionMapSubcriber = this.mapsService.getMissionMap();
    this.missionMapSubcriber.subscribe({
      next: missionMap => {
        this.missionMap = missionMap;
      },
      error: err => this.errorMessage = err
    });
  }

  ngOnInit(): void {
    this.getAllMaps();
    this.getMissionMap();
  }

  private removeMap(mapToDelete: string) {
    this.actualMaps.forEach((value, index) => {
      if (value == mapToDelete) {
        this.actualMaps.splice(index, 1);
        this.lastSelectedIndex = index;
      }
    });

    if (this.lastSelectedIndex === 0) {
      this.selectedMap = this.actualMaps[0];
    }
    else {
      this.selectedMap = this.actualMaps[this.lastSelectedIndex - 1]
    }
    this.imageSource = this.sanitizer.bypassSecurityTrustResourceUrl("");
  }

  private getImageTypeFromBase64String(base64String: string): string {

    let fileHeader = new Map();

    //get the first 3 char of base64
    fileHeader.set("/9j", "jpg")
    fileHeader.set("iVB", "png")
    fileHeader.set("Qk0", "bmp")
    fileHeader.set("SUk", "tiff")

    let res = ""

    fileHeader.forEach((v, k) => {
      if (k == base64String.substr(0, 3)) {
        res = v
      }
    })

    //if file is not supported
    if (res == "") {
      res = "unknown file"
    }

    //return map value
    return res;
  }
}


