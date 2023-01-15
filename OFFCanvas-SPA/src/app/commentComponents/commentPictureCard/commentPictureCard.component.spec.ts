import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentPictureCardComponent } from './commentPictureCard.component';

describe('CommentPictureCardComponent', () => {
  let component: CommentPictureCardComponent;
  let fixture: ComponentFixture<CommentPictureCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommentPictureCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentPictureCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
