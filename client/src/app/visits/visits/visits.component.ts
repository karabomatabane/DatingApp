import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { Visitor } from 'src/app/_models/visitor';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-visits',
  templateUrl: './visits.component.html',
  styleUrls: ['./visits.component.css']
})
export class VisitsComponent implements OnInit {
visitors: Partial<Visitor[]>;
lastvisit: Date;
predicate = 'visitedBy';
pageNumber = 1;
pageSize = 5;
filter = false;
pagination: Pagination;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadVisits();
  }

  loadVisits() {
    this.memberService.getVisits(this.predicate, this.pageNumber, this.pageSize, this.filter).subscribe(response => {
      this.visitors = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadVisits();
  }

  // onFilter(value: boolean){
  //   this.filter = value;
  // }

}
