import { TestBed } from '@angular/core/testing';

import { MapEntitiesService } from './map-entities.service';

describe('MapEntitiesService', () => {
  let service: MapEntitiesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MapEntitiesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
