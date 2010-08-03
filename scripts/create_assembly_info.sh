#!/bin/sh
# Creates the AssemblyInfo.cs used by the projects.

if [ -f "$1/AssemblyInfo.cs.tpl" ] ; then

	(cat <<'EOF'
/*
 * This is file has been automatically generated from the AssemblyInfo.cs.tpl
 * file by the create_assembly_info.sh script.
 *
 * Please do not modify this file, instead, modify the template where
 * appropriate.
 */
EOF
	) > "$1/AssemblyInfo.cs"
	sed "s/ID/`hg id`/g" "$1/AssemblyInfo.cs.tpl" | sed "s/DATE/`hg parents --template '{date|date}'`/g" >> "$1/AssemblyInfo.cs"
	echo "Created AssemblyInfo.cs"
else
	echo "Couldn't find $1/AssemblyInfo.cs.tpl!"
	exit 1
fi

