/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { PictureService } from './picture.service';

describe('Service: Picture', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PictureService]
    });
  });

  it('should ...', inject([PictureService], (service: PictureService) => {
    expect(service).toBeTruthy();
  }));
});
