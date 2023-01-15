import { Component, OnInit, Input } from '@angular/core';
import { Report } from 'src/app/_models/Report';

@Component({
  selector: 'app-report-card',
  templateUrl: './report-card.component.html',
  styleUrls: ['./report-card.component.css']
})
export class ReportCardComponent implements OnInit {
  @Input() report: Report;

  constructor() { }
  ngOnInit(): void {}
}
