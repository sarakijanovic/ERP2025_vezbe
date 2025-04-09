import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import Chart from 'chart.js';

// core components
import {
  chartOptions,
  parseOptions,
  chartExample1,
  chartExample2
} from "../../variables/charts";
import { WatercoolerDTO } from 'src/app/DTOs/watercoolerDTO';
import { WatercoolerType } from 'src/app/models/watercoolerType';
import { WatercoolerTypeService } from 'src/app/services/watercoolerTypeService/watercooler-type.service';
import { WatercoolerService } from 'src/app/services/watercoolerService/watercooler.service';
import { MatDialog } from '@angular/material/dialog';
import { WatercoolerInfoDialogComponent } from 'src/app/components/dialogs/watercoolerInfoDialog/watercooler-info-dialog/watercooler-info-dialog.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  sortByPrice: string;
  typeId: string;
  companyName: string = '';
  currentPage: number;
  disabledForward: boolean;
  watercoolers: WatercoolerDTO[];
  watercoolerTypes: WatercoolerType[];
  watercoolerInfo: WatercoolerDTO;
  
  constructor(private watercoolerService: WatercoolerService,
              private watercoolerTypeService: WatercoolerTypeService,
              private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.startSubscription();
  }

  startSubscription() {
    this.watercoolerService.getAllWatercoolersSorting(1,6).subscribe(res => 
      {
        this.watercoolers = res;
        this.currentPage = 1;
      });
    this.watercoolerTypeService.getAllWatercoolerTypes().subscribe(res => this.watercoolerTypes = res
    )
  }

  getWatercoolerInfo(id: string) {
    this.watercoolerService.getWatercoolerById(id).subscribe(res => 
      {
        this.watercoolerInfo = res;
        this.watercoolerTypeService.getWatercoolerTypeById(res.tipAparataID).subscribe(res => 
        {
          this.watercoolerInfo.tipAparataNaziv = res.nazivTipa;
        })
        this.dialog.open(WatercoolerInfoDialogComponent, {
          width: '600px',
          data: {
            watercooler: this.watercoolerInfo
          }
        });
      });
  }

  changePage(page: number) {
    if (page >= 1) {
      this.watercoolerService.getAllWatercoolersSorting(page, 6, this.sortByPrice, this.typeId, this.companyName).subscribe(res => 
        {
          if(res != null) {
            this.currentPage = page;
            this.watercoolers = res;
            if(this.watercoolers.length < 6)
              {
                this.disabledForward = true;
              } else {
                this.disabledForward = false;
              }
          } else {
            this.disabledForward = true;
          }
        });
    }
  }

  sortWatercoolersByPrice() {
    if(this.sortByPrice == "asc")
    {
      this.sortByPrice = "desc"
    } else {
      this.sortByPrice = "asc"
    }
    this.watercoolerService.getAllWatercoolersSorting(1, 6, this.sortByPrice, this.typeId, this.companyName).subscribe( res => {
      this.watercoolers = res;
      this.currentPage = 1;
    })
  }

  filterByType(typeId:string) {
    this.typeId = typeId;
    this.watercoolerService.getAllWatercoolersSorting(1, 6, this.sortByPrice, this.typeId, this.companyName).subscribe( res => {
      this.watercoolers = res;
      this.currentPage = 1;
    })
  }

  filterByCompany() {
    this.watercoolerService.getAllWatercoolersSorting(1, 6, this.sortByPrice, this.typeId, this.companyName).subscribe( res => {
      this.watercoolers = res;
      this.currentPage = 1;
    })
  }

  clearFilters() {
    this.startSubscription();
    this.typeId = null;
    this.sortByPrice = null;
    this.companyName = null;
  }
}
