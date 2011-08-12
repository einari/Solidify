require 'rake/clean'

module GlobalSettings
  $compiler = "dmcs"
  $core_references = ["mscorlib.dll", "System.dll"]
  $output_folder = "./"
end

class Target
  include GlobalSettings
  include Rake::DSL
  
  attr_accessor :description
  attr_accessor :folder
  attr_accessor :file
  attr_accessor :system_references
  attr_accessor :third_party_references
  attr_accessor :project_references
  attr_accessor :files
  attr_accessor :sources
  
  def initialize(&block)
    instance_eval(&block)
  end
  
  def prepare
    @sources = File.expand_path(@folder)
    @files = Dir["#{sources}/**/*.cs"]
  end
  
  def output_file
    $output_folder + "/" + @file
  end
  
  def build
    references = $core_references + @system_references + @third_party_references;
    joined_references = references.join(",")
    joined_references +=  ",#{$output_folder}/"+@project_references.join(",#{$output_folder}/") unless @project_references == nil
    
    joined_files = @files.join(" ")
    sh "#{$compiler} -out:#{output_file} -target:winexe -reference:#{joined_references} #{joined_files}"
  end
end

desc "Build all of Bifrost"
task :build do
  targets = [ 
      Target.new { 
          @description = "Solidify"
          @folder = "Solidify"
          @file = "Solidify.exe"
          @system_references = ["System.ServiceModel.dll", "System.Configuration.dll", "System.Xml.Linq.dll", "System.ComponentModel.DataAnnotations.dll"]
          @third_party_references = [] #["Libraries/CommonServiceLocator/dotNet/Microsoft.Practices.ServiceLocation.dll", "Libraries/Castle/dotNet/Castle.Core.dll", "Libraries/Castle/dotNet/Castle.DynamicProxy2.dll", "Libraries/FluentValidation/dotNet/FluentValidation.dll","Libraries/log4net/log4net.dll","Libraries/AutoMapper/dotNet/AutoMapper.dll","Libraries/JSON.net/DotNet/Newtonsoft.Json.dll"]
        },
    ]
  
  targets.each do |target|
    puts "Compiling : #{target.description}"

    target.prepare
    
    desc target.description
    file target.output_file => target.files do; target.build; end
    Rake::Task[target.output_file].invoke
  end
end


task :default => [:build]

