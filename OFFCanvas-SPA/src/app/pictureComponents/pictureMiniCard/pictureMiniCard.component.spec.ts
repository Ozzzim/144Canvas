import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PictureMiniCardComponent } from './picture-mini-card.component';

describe('PictureMiniCardComponent', () => {
  let component: PictureMiniCardComponent;
  let fixture: ComponentFixture<PictureMiniCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PictureMiniCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PictureMiniCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
