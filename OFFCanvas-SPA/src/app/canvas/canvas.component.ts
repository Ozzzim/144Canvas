import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import {AuthService} from '../_services/auth.service'
import { PictureService } from '../_services/picture.service';
//import * as p5 from 'p5';

@Component({
  selector: 'app-canvas',
  templateUrl: './canvas.component.html',
  styleUrls: ['./canvas.component.scss']
})

export class CanvasComponent implements OnInit {
  model: any = {};
  //HTML Objects
  canvas : any;
  ctx : any;
  cData: any;
  brushSizeField : any;
  colorField: any;
  //Tools
  color: string = "#000000";
  tool: Tool = Tool.Draw;
  brushSize: number =1;
  isMouseDown: boolean=false;
  touched: boolean=false;
  pos: any;

  lastPosX: number =-1;
  lastPosY: number =-1;

  xOffset: number =0;
  yOffset: number =0;

  /*//p5
  private p5;
  private sketch(p: any) {
    p.setup = () => {
      p.createCanvas(256, 144);//.parent("canvasDisplay");
      p.noLoop();
      console.log('Setup');
    };
  
    p.draw = () => {
      p.background(255);
      p.fill(0);
      p.rect(p.width / 2, p.height / 2, 50, 50);
    };
  }*/


  constructor(private pictureService: PictureService,private authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.canvas= document.getElementById("Canvas");
    this.ctx=this.canvas.getContext("2d");
    this.cData=this.ctx.getImageData(0,0,256,144);
    this.brushSizeField = document.getElementById("brushSizeField");
    //this.brushSizeField.value=this.brushSize;
    this.colorField = document.getElementById("colorField");
    this.setSize(this.brushSize);
    //this.colorField.value=this.color;

    
    this.canvas.addEventListener("mousemove",this.onMove.bind(this));
    this.canvas.addEventListener("mousedown",this.mouseDown.bind(this));
    this.canvas.addEventListener("mouseup",this.mouseUp.bind(this));
    this.canvas.addEventListener("mouseleave",this.mouseUp.bind(this));

    //Canvas test
    this.ctx.fillStyle="#ffffff";
    this.ctx.fillRect(0,0,256,144);
    this.ctx.fillStyle="#000000";
    //this.p5 = new p5(this.sketch);
  }

  async post(){
    if(this.authService.loggedIn()){
      if(this.touched){
        this.model.url=this.canvas.toDataURL();
        /*this.model.imageData = 
        this.model.imageData=new Uint8Array(await new Promise(resolve => (
          new Promise(resolve => this.canvas.toBlob(resolve))
        ).arrayBuffer()));*/
        //var imageBlob:Blob=await new Promise(resolve => this.canvas.toBlob(resolve))
        //this.model.imageData=new File([imageBlob],this.model.title, {type : imageBlob.type});
        /*this.canvas.toBlob(function(blob){
          this.alertify.message(blob.type);
          this.model.imageData=blob;
        });//*/
;
        this.model.userid=+this.authService.decodedToken.nameid;
        this.pictureService.postPicture(this.model).subscribe(next=>{
          this.alertify.message("Success! Your image has been submited");
          this.router.navigate(['/pictures'])//Change to redirect to the image
        }, error => {
          console.log(error);
          this.alertify.error('Post failed... '+error);
        }
        );
      }else
        this.alertify.error("You can't submit an empty canvas!");
    }else{
      this.alertify.error("You're not logged in!");
    }
  }

  ///*
  mouseUp(){
    //console.log(e);
   this.isMouseDown=false;
   //this.ctx.save();
  }

  mouseDown(e){
    this.isMouseDown=true;
    this.onDraw(e,true);
    //this.Line(0,0,255,144);
    //this.setLastPos(e);
    //onDraw(getMousePos(e,document.getElementById("Canvas")),this);
  }
  ///*
  onMove(e){
    if(this.isMouseDown){
      var rect = this.canvas.getBoundingClientRect(), // abs. size of element
      scaleX = this.canvas.width / rect.width,    // relationship bitmap vs. element for X
      scaleY = this.canvas.height / rect.height;  // relationship bitmap vs. element for Y
  
      
      var newX = Math.round((e.clientX - rect.left) * scaleX);
      var newY = Math.round((e.clientY - rect.top) * scaleY);
      //console.log("Line("+this.pos.x+","+this.pos.y+","+newX+","+newY+")");
      this.Line(this.pos.x,this.pos.y,newX,newY);

      this.pos = {
        x: newX,
        y: newY
      }  
      //this.onDraw(e,false);
    }
  }//*/

  setDraw(){
    this.tool=Tool.Draw;
  }
  isDraw():boolean{
    return this.tool==Tool.Draw;
  }
  setFill(){
    this.tool=Tool.Fill;
  }
  isFill():boolean{
    return this.tool==Tool.Fill;
  }
  setSample(){
    this.tool=Tool.Sample;
  }
  isSample():boolean{
    return this.tool==Tool.Sample;
  }
  setSize(size: number){//size: number){
    if(size<=0)
      this.brushSize=1
    else
      this.brushSize=size;

    //if(size!=0)
    this.brushSizeField.value=this.brushSize;
  }
  setColor(color: string){
    //Add color validation
    this.color=color;
    this.ctx.fillStyle=color;
    this.colorField.value=color;
  }

  setMousePos(e) {
    var rect = this.canvas.getBoundingClientRect(), // abs. size of element
      scaleX = this.canvas.width / rect.width,    // relationship bitmap vs. element for X
      scaleY = this.canvas.height / rect.height;  // relationship bitmap vs. element for Y

    var newX = Math.round((e.clientX - rect.left) * scaleX);
    var newY = Math.round((e.clientY - rect.top) * scaleY);
    this.pos = {
      x: newX,//Math.round((e.clientX - rect.left) * scaleX),   // scale mouse coordinates after they have
      y: newY//Math.round((e.clientY - rect.top) * scaleY)     // been adjusted to be relative to element
    }    
  }

  /*onDraw(pos){*/
  onDraw(e,allowFill:boolean){
    this.setMousePos(e);
    //console.log((this.canvas==null)+"nullcheck"+(document.getElementById("Canvas")==null));
    //var pos = getMousePos(e,document.getElementById("Canvas"));//this.canvas);
    //
    if(this.isMouseDown)
      //console.log(this.pos.x+" "+this.pos.y);
      switch(this.tool) { 
        case Tool.Draw:{
          this.sDraw(this.pos.x,this.pos.y, this.brushSize);
          if(!this.touched)
            this.touched=true;
          break;
        }
        case Tool.Fill:{
          if(allowFill)
            this.Fill(this.pos.x,this.pos.y);
          this.isMouseDown=false;
          break;
        }
        case Tool.Sample:{
          this.Sample(this.pos.x,this.pos.y, this.brushSize);
          this.isMouseDown=false;
          break;
        }
        default:{
          this.tool=Tool.Draw;
        }
      }
  }

  sDraw(posX: number, posY: number, size: number){
    this.ctx.fillRect(posX-Math.round(size/2)+this.xOffset,posY-Math.round(size/2)+this.yOffset,size,size);
    //console.log("sDraw "+posX);
  }

  Line(posXS:number,posYS:number,posXE:number,posYE:number){
    var curX:number = posXS;
    var curY:number = posYS;
    var dx:number = posXE - posXS;
    var dy:number = posYE - posYS;
    var d:number = Math.abs(dx)>=Math.abs(dy) ? Math.abs(dx) : Math.abs(dy);
    var i:number = 1;

    dx=dx/d;
    dy=dy/d;
    //console.log("DDA: "+d+","+dx+","+dy);
    while(i<=d){
      this.sDraw(Math.floor(curX),Math.floor(curY),this.brushSize);
      curX=curX+dx;
      curY=curY+dy;
      i++;
    }

    return;
  }
  Fill(posX: number, posY: number){
    this.cData=this.ctx.getImageData(0,0,256,144);
    var filledColor:number[]=this.GetIDataColor(posX,posY);
    var fillingColor:number[]=this.GetHexToRGB(this.color);

    var currentPos:number[]=[0,0];
    var currentColor:number[];
    var queue=[];
    var checklist:Set<number>=new Set();
    queue.push([posX,posY]);

    while(queue.length>0){
      //console.log(queue.length);
      currentPos=queue.pop();
      if(checklist.has(this.GetIDataStartingPoint(currentPos[0],currentPos[1])))
        continue
      if(currentPos[0]>=this.canvas.width || currentPos[0]<0 || currentPos[1]>=this.canvas.height || currentPos[1]<0){
        //console.log("Quit at range check: ("+(currentPos[0]>=this.canvas.width)+","+(currentPos[0]<0)+","+(currentPos[1]>=this.canvas.height)+","+(currentPos[1]<0)+")"+currentPos[0]+" "+this.canvas.height);
        continue;
      }
      currentColor=this.GetIDataColor(currentPos[0],currentPos[1]);
      if(!this.CompareColors(filledColor,currentColor)){
        continue;
      }
      this.SetPixel(currentPos[0],currentPos[1],fillingColor);
      checklist.add(this.GetIDataStartingPoint(currentPos[0],currentPos[1]));

      queue.push([currentPos[0],currentPos[1]+1]);
      queue.push([currentPos[0]+1,currentPos[1]]);
      queue.push([currentPos[0],currentPos[1]-1]);
      queue.push([currentPos[0]-1,currentPos[1]]);
    }
  }
  Sample(posX: number, posY: number, size: number){
    
    var index:number=this.GetIDataStartingPoint(posX,posY);
    this.cData=this.ctx.getImageData(0,0,256,144);
    this.setColor(this.GetRGBToHex( 
        this.cData.data[index],
        this.cData.data[index+1],
        this.cData.data[index+2]
    ));
  }

  GetIDataStartingPoint(xPos:number,yPos:number):number{
    return (xPos+(yPos*this.canvas.width))*4;
  }

  GetIDataColor(xPos:number,yPos:number):number[]{
    var pos=(xPos+(yPos*this.canvas.width))*4;
    return [this.cData.data[pos],this.cData.data[pos+1],this.cData.data[pos+2]];
  }

  GetRGBToHex(r:number,g:number,b:number):string{
    var stringify = [r.toString(16),g.toString(16),b.toString(16)];
    for(var i=0;i<stringify.length;i++)
      if(stringify[i].length<=1)
        stringify[i]="0"+stringify[i];
    return "#"+stringify[0]+stringify[1]+stringify[2];
    //return "#"+r.toString(16)+g.toString(16)+b.toString(16);
  }

  GetHexToRGB(hex:string):number[]{
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return [parseInt(result[1], 16),parseInt(result[2], 16),parseInt(result[3], 16)];
  }

  CompareColors(colorA:number[],colorB:number[]):boolean{
    return colorA[0]==colorB[0] && colorA[1]==colorB[1] && colorA[2]==colorB[2];
  }

  SetPixel(xPos:number,yPos:number,color:number[]){//Assumes cData has been updated beforehand    
    this.ctx.fillRect(xPos,yPos,1,1);
  }
}

enum Tool {
  Draw,
  Fill,
  Sample,
}