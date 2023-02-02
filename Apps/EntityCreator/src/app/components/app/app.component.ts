import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  navLinks: any[];
  activeLinkIndex = -1; 
  constructor(private router:Router){
    this.navLinks = [
      {
          label: 'MapEntities',
          link: './mapentities',
          index: 0
      }, {
          label: 'Maps',
          link: './maps',
          index: 1
      }
    ] 
  }
  ngOnInit(): void {
    this.router.events.subscribe((res) => {
        this.activeLinkIndex = this.navLinks.indexOf(this.navLinks.find(tab => tab.link === '.' + this.router.url));
    });
  }
  title = 'test-app2';
}
