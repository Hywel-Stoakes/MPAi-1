﻿#!/usr/bin/perl
use strict;
use File::Basename;
use File::Copy qw(move);
use warnings;
use Encode;

die "Usage: $0 <input dir> <extion>\n" unless @ARGV == 2;
my $InputDir = $ARGV[0] ;
my $Ext = $ARGV[1] ;

my @WordList = (
            "tënei",    #01
            "täne",     #02
            "hau",      #03
            "hou",      #04
            "pao",      #05
            "pau",      #06
            "pou",      #07
            "pö",       #08
            "pai",      #09
            "pae",      #10
            "kë",       #11
            "kei",      #12
            "kï",       #13
            "hë",       #14
            "hei",      #15
            "hï",       #16
            "tae",      #17
            "tai",      #18
            "mätao",    #19
            "mätau",    #20
            "mätou",    #21
            "toetoe",   #22
            "toi",      #23
            "hoihoi",   #24
            "hoe",      #25
            "mao",      #26
            "mau",      #27
            "moutere",  #28
            "tü",       #29
            "matiu"     #30
);

my %SpeakerHash = (
  "K004M" => "male",
  "K005M" => "male",
  "K006M" => "male",
  "K007M" => "male",
  "K008M" => "male",
  "K009M" => "male",
  "K010M" => "male",

  "L1Y01M" => "male",
  "L1Y02M" => "male",
  "L1Y03M" => "male",
  "L1Y04M" => "male",
  "L1Y05M" => "male",

  "L1H01M" => "female",
  "L1H02M" => "female",
  "L1H03M" => "female",
  "L1H04M" => "female",
  "L1H05M" => "female",
  "L1H06M" => "female",

  "R001M" => "female",
  "R002M" => "female",
  "R003M" => "female",
  "R004M" => "female",
  "R005M" => "female",
  "R006M" => "female",
  "R007M" => "female",
  "R008M" => "female"
);

opendir(DH, "$InputDir") or die "Can't open: $!\n" ;
#load all the files in $InputDir which have suffix "wav"
my @list = grep {/$Ext$/ && -f "$InputDir/$_" } readdir(DH) ;
closedir(DH) ;
chdir($InputDir) or die "Can't cd dir: $!\n" ;
foreach my $file (@list){
    my $Basename = basename($file,($Ext));

    my @parts=split(/_/,$Basename);
    if(@parts == 2){
        my $NewFile = $SpeakerHash{$parts[0]}."-word-".$WordList[$parts[1] - 1]."-".$parts[0].".".$Ext;
        if(-e $NewFile){
            warn "Duplicate name after renaming!";
            next;
        } else {
            #rename($file, $NewFile);
            move $file, $NewFile;
        }
    } else {
        warn "Invalid name format!";
        next;
    }
}