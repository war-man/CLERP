import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ProductTypeResponse } from '@_generated/models';
import { ProductTypeService } from '@_generated/services';

@Component({
  selector: 'app-productTypes',
  templateUrl: './productTypes.component.html',
  styleUrls: ['./productTypes.component.scss']
})
export class ProductTypesComponent implements OnInit {

  public searchControl: FormControl = new FormControl('');
  public productTypes: Array<ProductTypeResponse>;
  public filteredProductTypes: Array<ProductTypeResponse>;


  constructor(private productTypeService: ProductTypeService) { }


  ngOnInit() {
    this.getAllProducts();
  }


  private getAllProducts() {
    this.productTypeService.GetAllProductTypes().subscribe(data => {
      this.productTypes = data.productTypes.sort(function(a, b) {
        var x = a.name.toLowerCase();
        var y = b.name.toLowerCase();
        if (x < y) {return -1;}
        if (x > y) {return 1;}
        return 0;
      });
      this.filteredProductTypes = this.productTypes;
    });
  }

  searchClicked(): void {
    var searchText = this.searchControl.value.toLowerCase();
    this.filteredProductTypes = this.productTypes.filter(pt => {
      return (
        pt.Description.toLowerCase().includes(searchText)
        || pt.ean.toLowerCase().includes(searchText)
        || pt.name.toLowerCase().includes(searchText)
      );
    })
  }

  deleteClicked(id: string) : void {
    this.productTypeService.DeleteProductType(id).subscribe(data => {
      alert("ProductType with id: '" + id + "' was deleted succesfully");

      this.getAllProducts();
    }, error => {
      alert("Could not delete ProductType")
    });
  }
}
